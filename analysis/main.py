import argparse
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
        try:
            df = pd.read_csv(args.dataset_path, encoding='latin1')
            df_head = df.head(6)
            column_types = df.dtypes.to_dict()
            context = f"These are the first 6 rows of the dataset:\n{df_head}\n\nThese are the column types:\n{column_types}\n\nAllowed chart types are: Bar, Line, Scatter, Bubble, Pie, Doughnut, PolarArea, Radar. The name of the dataset is {args.dataset_path}"
            user_input = f"{args.prompt}\nWhat are the all columns (max 3) I can make a graph with and what type of graph should I use? May be more. Please respond in the format 'column_name:chart_type'."
            
            response = raw_chat_with_gpt_without_cache(context, user_input)
            # Parse the response to get column names and chart types
            columns_and_charts = response.split(',')
            columns = []
            chart_types = {}
            for col in columns_and_charts:
                if ':' in col:
                    column_name, chart_type = col.split(':')
                    column_name = column_name.strip().strip("'")  # Clean up the column name
                    columns.append(column_name)
                    chart_types[column_name] = chart_type.strip()

            # Preprocess the data
            preprocessed_data = {}
            for column in columns:
                col_data = df[[column]].dropna()  # Remove rows with missing values
                col_data = col_data.drop_duplicates()  # Remove duplicate rows
                col_data = col_data.convert_dtypes()  # Ensure consistent data types
                preprocessed_data[column] = col_data[column].tolist()

            # Prepare the JSON response
            json_response = {
                "visualization": {
                    "columns": columns,
                    "chart_types": chart_types,
                    "data": preprocessed_data,
                },
                "cacheId": None
            }

            return json.dumps(json_response)
        except UnicodeDecodeError as e:
            print(f"Error reading the CSV file: {e}")

if __name__ == "__main__":
    response = main()
    print(response)
