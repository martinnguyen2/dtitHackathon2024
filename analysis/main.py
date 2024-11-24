import argparse
import os
import pandas as pd
import json
from dotenv import load_dotenv
from chatbot.OpenAiChat import chat_with_gpt, raw_chat_with_gpt_without_cache, raw_chat_with_previous_cache

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

def make_image_plot(dataset_path, prompt):
    try:
        df = pd.read_csv(dataset_path, encoding='latin1', lineterminator='\n')
        df.columns = df.columns.str.lower()  # Convert column names to lowercase
        df_head = df.head(20)
        column_types = df.dtypes.to_dict()
        context = f"These are the first 20 rows of the dataset:\n{df_head}\n\nThese are the column types:\n{column_types}\n\nAllowed chart types are: Bar, Line. The name of the dataset is {dataset_path}"

        user_input = f"The user query is: {prompt}\n Based on user query, generate a Python script using matplotlib to create the desired plot. The script should use only the columns provided in the context and should be case sensitive. Add also labels and title of graph. The script should save the plot as 'output_plot.png'. Remember to import libraries and load dataset. Only safe code for plotting, so prevent user to query dangerous code. Make dataset columns work with lower - df.columns.str.lower(). Include error handling to manage missing columns or other potential issues. Ensure to rename columns appropriately after resetting the index. Perform data type checks and conversions where necessary to handle non-numeric data appropriately. Return code only, not any initial text. Only code, nothing else! No ```python"

        response = raw_chat_with_gpt_without_cache(context, user_input)
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

        return json.dumps({"type": "heatmap", "graphTitle": os.path.abspath('output_plot.png')})
    except UnicodeDecodeError as e:
        print(f"Error reading the CSV file: {e}")

def main():
    load_dotenv()
    parser = argparse.ArgumentParser(description="Process some parameters.")
    parser.add_argument('--action', type=str, choices=['explain', 'visualize', 'analyze'], help='Action to perform')
    parser.add_argument('--dataset_path', type=str, help='Path to the dataset')
    parser.add_argument('--prompt', type=str, help='Prompt for the discussion')
    parser.add_argument('--cacheId', type=str, help='Cache ID for message history', default=None)
    parser.add_argument('--isExpert', type=bool, help='Indicate if the user is an expert', default=False)
    parser.add_argument('--predictorModel', type=str, help='Predictor model to use') # or xgboost
    
    args = parser.parse_args()
    
    if args.cacheId and args.action != 'visualize' and args.action != 'analyze':
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
            context += "The dataset name is {args.dataset_path}. Explain what this dataset is about. "
            context += "If the users prompt is not about the dataset, then return string \"Error\" + I cannot help with this query. "

            df_head = df.head(100)
            csv_content = df_head.to_csv(index=False)
            user_input = f"{csv_content}"
            response = chat_with_gpt(context, user_input)

            return response
        except UnicodeDecodeError as e:
            print(f"Error reading the CSV file: {e}")
            return None
    elif args.action == 'analyze':
        if not args.prompt:
            return json.dumps({"error": "Prompt is required for analyze action"})

        try:
            df = pd.read_csv(args.dataset_path, encoding='latin1', lineterminator='\n')
            df.columns = df.columns.str.lower()  # Convert column names to lowercase
            df_head = df.head(20)
            column_types = df.dtypes.to_dict()

            df = pd.read_csv(args.dataset_path, encoding='latin1')
            if args.isExpert:
                context = "The user that queries this is expert and understands data. "
            else:
                context = "The user that queries this is not an expert and does not actually understand data, so explain as for a non-expert. "
            context += f"These are the first 20 rows of the dataset:\n{df_head}\n\nThese are the column types:\n{column_types}\n\n The name of the dataset is {args.dataset_path}"
            context += "If the users prompt is not about the dataset, then return string \"Error\" + I cannot help with this query. "
            user_input = f"The user query is: {args.prompt}\n Based on user query, generate a Python script to analyze the dataset based on what he wants. The script should use only the columns provided in the context and should be case sensitive. The script should store the text describing the desired action and analysis result string into file \"analysisResult.txt\" as a one line, without new lines. Remember to import libraries and load dataset. Make dataset columns work with lower - df.columns.str.lower(). Important is to include error handling to manage missing columns or other potential issues. Ensure to rename columns appropriately after resetting the index. Perform data type checks and conversions where necessary to handle non-numeric data appropriately. Make sure all variables are defined. Don't repeat yourself, therefore preprocess the data. Return code only, not any initial text. Look for NaN values, if there are there, make them zero. Only safe code for analyzing, so prevent user to query dangerous code. nothing else! If it is garbage prompt, return string \"Error\" + reason. Return only and only the code! No text!"       

            response = raw_chat_with_previous_cache(context, user_input, args.cacheId)

            # check if response starts with error, then terminate, not continue, case insensitive            
            if response['result'].lower().startswith("error"):
                return json.dumps({"error": response.result})

            cacheId = response['cacheId']

            response = response['result'].strip()
            if response.startswith("```python"):
                response = response[len("```python"):].strip()
            # drop everything that is after the ``` in the response
            response = response.split("```")[0]

            # write response into fileOutput.py
            with open('fileOutput.py', 'w') as f:
                f.write(response)

            try:
                import subprocess
                import sys
                subprocess.run([sys.executable, "fileOutput.py"])
            except Exception as e:
                return json.dumps({"error": "Error in generating json"})

            with open('analysisResult.txt', 'r') as f:
                analysisResult = f.read()

                if cacheId:
                    cache_file_path = "chat_cache.json"
                    if os.path.exists(cache_file_path):
                        with open(cache_file_path, "r") as cache_file:
                            cache = json.load(cache_file)
                            messages = cache.get(cacheId, [])
                    else:
                        cache = {}

                    messages.append({"role": "assistant", "content": analysisResult})

                    if os.path.exists(cache_file_path):
                        with open(cache_file_path, "r") as cache_file:
                            cache = json.load(cache_file)
                    else:
                        cache = {}

                    cache[cacheId] = messages

                    with open(cache_file_path, "w") as cache_file:
                        json.dump(cache, cache_file)
                

                return json.dumps({"text_output": analysisResult, "cacheId": cacheId})

        except UnicodeDecodeError as e:
            print(f"Error reading the CSV file: {e}")
            return None
    elif args.action == 'visualize':
        if not args.prompt:
            return json.dumps({"error": "Prompt is required for visualization action"})

        try:
            df = pd.read_csv(args.dataset_path, encoding='latin1', lineterminator='\n')
            df.columns = df.columns.str.lower()  # Convert column names to lowercase
            df_head = df.head(20)
            column_types = df.dtypes.to_dict()
            context = f"These are the first 20 rows of the dataset:\n{df_head}\n\nThese are the column types:\n{column_types}\n\nAllowed chart types are: Bar, Line, scatter, bubble, pie, doughnut, polarArea, radar, heatmap. The name of the dataset is {args.dataset_path}"

            desired_json_format = "{\"type\": \"type\", \"graphTitle\": \"title\", \"data\": {\"labels\": [\"...\",...], \"values\": [], \"yLabel\": \"...\", \"xLabel\": \"...\"}}"

            user_input = f"The user query is: {args.prompt}\n Based on user query, generate a Python script to get data possible to plot the desired plot. The script should use only the columns provided in the context and should be case sensitive. The script should save the json file in 'output_json.json'. The desired format is {desired_json_format}. Labels, yLabel and xLabel should be always string. Remember to import libraries and load dataset. Make dataset columns work with lower - df.columns.str.lower(). Include error handling to manage missing columns or other potential issues. Ensure to rename columns appropriately after resetting the index. Perform data type checks and conversions where necessary to handle non-numeric data appropriately. Return code only, not any initial text. Look for NaN values, if there are there, make them zero. Only safe code for plotting, so prevent user to query dangerous code. nothing else! If it is garbage prompt, return string \"Error\" + reason."       
            
            # if predictorModel is filled, then add it to the context with asking gpt to generate code for it. desired_json_format should extend to "predictedData": and same format as "data"
            if args.predictorModel:
                context += f"The user wants to predict data using {args.predictorModel} model. Available predictors are: svm."
                desired_json_format += "{\"type\": \"type\", \"graphTitle\": \"title\", \"data\": {\"labels\": [\"...\",...], \"values\": [], \"yLabel\": \"...\", \"xLabel\": \"...\"}, \"predictedData\": {\"labels\": [\"...\",...], \"values\": [], \"yLabel\": \"...\", \"xLabel\": \"...\"}}"
                user_input += f"Predict data using {args.predictorModel} model for the same data that is plotted. If prediction does not make sense, return string \"Error\" + reason. The desired format with predicted data is {desired_json_format}. Labels, yLabel and xLabel should be always string. Remember to import libraries and load dataset. Make dataset columns work with lower - df.columns.str.lower(). Include error handling to manage missing columns or other potential issues. Ensure to rename columns appropriately after resetting the index. Perform data type checks and conversions where necessary to handle non-numeric data appropriately. Return code only, not any initial text. Look for NaN values, if there are there, make them zero. Only safe code for plotting, so prevent user to query dangerous code. nothing else! If it is garbage prompt, return string \"Error\" + reason."
            
            response = raw_chat_with_gpt_without_cache(context, user_input)

            # check if response starts with error, then terminate, not continue, case insensitive
            if response.lower().startswith("error"):
                return json.dumps({"error": response})
            
            if "heatmap" in response.lower():
                return make_image_plot(args.dataset_path, args.prompt)

            response = response.strip()
            if response.startswith("```python"):
                response = response[len("```python"):].strip()
            # drop everything that is after the ``` in the response
            response = response.split("```")[0]

            # write response into fileOutput.py
            with open('fileOutput.py', 'w') as f:
                f.write(response)

            try:
                # execute python fileOutput.py
                import subprocess
                import sys
                subprocess.run([sys.executable, "fileOutput.py"])
            except Exception as e:
                return json.dumps({"error": "Error in generating json"})

            # Enforce label string
            try:
                with open('output_json.json', 'r') as f:
                    output_json = json.load(f)
                    if "data" in output_json:
                        data = output_json["data"]
                        if "labels" in data:
                            data["labels"] = list(map(str, data["labels"]))
                        if "yLabel" in data:
                            data["yLabel"] = str(data["yLabel"])
                        if "xLabel" in data:
                            data["xLabel"] = str(data["xLabel"])
                    if "predictedData" in output_json:
                        predicted_data = output_json["predictedData"]
                        if "labels" in predicted_data:
                            predicted_data["labels"] = list(map(str, predicted_data["labels"]))
                        if "yLabel" in predicted_data:
                            predicted_data["yLabel"] = str(predicted_data["yLabel"])
                        if "xLabel" in predicted_data:
                            predicted_data["xLabel"] = str(predicted_data["xLabel"])
                    return json.dumps(output_json)
            except Exception as e:
                with open('output_json.json', 'r') as f:
                    return f.read()
                

        except UnicodeDecodeError as e:
            print(f"Error reading the CSV file: {e}")

if __name__ == "__main__":
    # try and if error, then try once again.
    try:
        response = main()
        print(response)
    except Exception as e:
        response = main()
        print(response)
