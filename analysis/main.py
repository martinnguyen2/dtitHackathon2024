import argparse
import os
import pandas as pd
import json
from dotenv import load_dotenv
from chatbot.OpenAiChat import chat_with_gpt, raw_chat_with_gpt_without_cache

def process_response(response):
    response_data = json.loads(response)
    if "possible_graphs" in response_data:
        possible_graphs = response_data["possible_graphs"]
        if possible_graphs:
            first_graph = possible_graphs[0]
            graph_data = {
                "graph_type": first_graph.get("type"),
                "graph_data": first_graph.get("data")
            }
            return json.dumps({
                "possible_graphs": possible_graphs,
                "selected_graph": graph_data
            })
        else:
            return json.dumps({"error": "No possible graphs found"})
    else:
        return response
    
def bar_line_chart(df, column_name_x, column_name_y, group_by, group_value, action, type):
    if group_by and group_value and group_by in df.columns and group_value.lower() != 'none':
        filtered_df = df[df[group_by].str.strip().str.lower() == group_value.strip().lower()]
    else:
        filtered_df = df

    if action == 'count':
        grouped_df = filtered_df.groupby(column_name_x).size().reset_index(name=f"{column_name_y}_count")
    else:
        grouped_df = filtered_df.groupby(column_name_x)[column_name_y].sum().reset_index(name=f"{column_name_y}_sum")

    graph_data = {
        "type": type,
        "data": {
            "labels": grouped_df[column_name_x].tolist(),
            "values": grouped_df[f"{column_name_y}_count" if action == 'count' else f"{column_name_y}_sum"].tolist(),
            "yLabel": column_name_y,
            "xLabel": column_name_x
        }
    }
    return graph_data

def main():
    load_dotenv()
    parser = argparse.ArgumentParser(description="Process some parameters.")
    parser.add_argument('--action', type=str, choices=['explain', 'visualize'], help='Action to perform')
    parser.add_argument('--dataset_path', type=str, help='Path to the dataset')
    parser.add_argument('--prompt', type=str, help='Prompt for the discussion')
    parser.add_argument('--cacheId', type=str, help='Cache ID for message history', default=None)
    parser.add_argument('--isExpert', type=bool, help='Indicate if the user is an expert', default=False)
    
    args = parser.parse_args()
    
    if args.cacheId and args.action != 'visualize':
        # Continue the conversation using the provided cacheId
        context = ""
        user_input = args.prompt
        response = chat_with_gpt(context, user_input, args.cacheId)
        return response
    elif args.action == 'explain':
        try:
            df = pd.read_csv(args.dataset_path, encoding='latin1')
            if args.isExpert:
                context = "The user that queries this is expert and understands data. "
            else:
                context = "The user that queries this is not an expert and does not actually understand data, so explain as for a non-expert. "
            context += "The dataset name is {args.dataset_path}. Explain what this dataset is about "
            df_head = df.head(100)
            csv_content = df_head.to_csv(index=False)
            user_input = f"{csv_content}"
            response = chat_with_gpt(context, user_input)

            return response
        except UnicodeDecodeError as e:
            print(f"Error reading the CSV file: {e}")
            return None
    elif args.action == 'visualize':
        if not args.prompt:
            return json.dumps({"error": "Prompt is required for visualization action"})

        make_plot = False

        if make_plot:
            try:
                df = pd.read_csv(args.dataset_path, encoding='latin1', lineterminator='\n')
                df.columns = df.columns.str.lower()  # Convert column names to lowercase
                df_head = df.head(20)
                column_types = df.dtypes.to_dict()
                context = f"These are the first 20 rows of the dataset:\n{df_head}\n\nThese are the column types:\n{column_types}\n\nAllowed chart types are: Bar, Line. The name of the dataset is {args.dataset_path}"

                user_input = f"The user query is: {args.prompt}\n Based on user query, generate a Python script using matplotlib to create the desired plot. The script should use only the columns provided in the context and should be case sensitive. The script should save the plot as 'output_plot.png'. Remember to import libraries and load dataset. Make dataset columns work with lower - df.columns.str.lower(). Return code only, not any initial text. Only code, nothing else! No ```python"       
                response = raw_chat_with_gpt_without_cache(context, user_input)


                # write response into fileOutput.py
                with open('fileOutput.py', 'w') as f:
                    f.write(response)
                
                # execute python fileOutput.py
                import subprocess
                import sys
                subprocess.run([sys.executable, "fileOutput.py"])

            except UnicodeDecodeError as e:
                print(f"Error reading the CSV file: {e}")
        else:
            try:
                df = pd.read_csv(args.dataset_path, encoding='latin1', lineterminator='\n')
                df.columns = df.columns.str.lower()  # Convert column names to lowercase
                df_head = df.head(20)
                column_types = df.dtypes.to_dict()
                context = f"These are the first 20 rows of the dataset:\n{df_head}\n\nThese are the column types:\n{column_types}\n\nAllowed chart types are: Bar, Line. The name of the dataset is {args.dataset_path}"

                desired_json_format = "{\"type\": type, \"data\": {\"labels\": [\"...\",...], \"values\": [], \"yLabel\": \"...\", \"xLabel\": \"...\"}}"

                user_input = f"The user query is: {args.prompt}\n Based on user query, generate a Python script to get data possible to plot the desired plot. The script should use only the columns provided in the context and should be case sensitive. The script should save the json file in 'output_json.json'. The desired format is {desired_json_format}. Labels are always string. Remember to import libraries and load dataset. Make dataset columns work with lower - df.columns.str.lower(). Return code only, not any initial text. Look for NaN values, if there are there, make them zero. Only safe code for plotting, so prevent user to query dangerous code. nothing else! If it is garbage prompt, return string \"Error\" + reason."       
                response = raw_chat_with_gpt_without_cache(context, user_input)

                # check if response starts with error, then terminate, not continue
                if response.startswith("Error"):
                    return json.dumps({"error": response})

                response = response.strip()
                if response.startswith("```python"):
                    response = response[len("```python"):].strip()
                if response.endswith("```"):
                    response = response[:-len("```")].strip()

                # write response into fileOutput.py
                with open('fileOutput.py', 'w') as f:
                    f.write(response)
                
                # execute python fileOutput.py
                import subprocess
                import sys
                subprocess.run([sys.executable, "fileOutput.py"])

                # Check for error if yes then return error json
                if not os.path.exists('output_json.json'):
                    return json.dumps({"error": "Error in generating json"})

                # I want return content of output_json.json
                with open('output_json.json', 'r') as f:
                    return f.read()

            except UnicodeDecodeError as e:
                print(f"Error reading the CSV file: {e}")

if __name__ == "__main__":
    response = main()
    print(response)
