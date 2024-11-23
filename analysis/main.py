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
        if not args.prompt:
            return json.dumps({"error": "Prompt is required for visualization action"})

        try:
            df = pd.read_csv(args.dataset_path, encoding='latin1')
            df_head = df.head(6)
            column_types = df.dtypes.to_dict()
            context = f"These are the first 6 rows of the dataset:\n{df_head}\n\nThese are the column types:\n{column_types}\n\nAllowed chart types are: Bar, Line, Scatter, Pie. The name of the dataset is {args.dataset_path}"
            user_input = f"The user query is: {args.prompt}\n Based on user query, what columns should I get from the dataset, which should be in x column, which should be in y column, which should be grouped? What graph he wants? If not specified, you decide which graph. You can use only the columns I provided, nothing else and those are case sensitive - so choose only from provided columns in context! Make it in format 'column_name_x:column_name_y:group_by:group_value:chart_type:action'. Action f.e. count if needed"
            
            response = raw_chat_with_gpt_without_cache(context, user_input)

            print(response)

            columns_and_charts = response.split(':')
            if len(columns_and_charts) != 6:
                return json.dumps({"error": "Invalid response format from GPT"})
            
            column_name_x, column_name_y, group_by, group_value, chart_type, action = columns_and_charts
            column_name_x = column_name_x.strip()
            column_name_y = column_name_y.strip()
            group_by = group_by.strip()
            group_value = group_value.strip()
            chart_type = chart_type.strip()
            action = action.strip()

            # Filter and preprocess the data
            if group_by and group_value and group_by in df.columns:
                filtered_df = df[df[group_by] == group_value]
            else:
                filtered_df = df

            if action == 'count':
                grouped_df = filtered_df.groupby(column_name_x).size().reset_index(name=column_name_y)
            else:
                grouped_df = filtered_df.groupby(column_name_x)[column_name_y].sum().reset_index()

            # Generate the graph data
            graph_data = {
                "type": chart_type,
                "data": {
                    "labels": grouped_df[column_name_x].tolist(),
                    "datasets": [{
                        "label": column_name_y,
                        "data": grouped_df[column_name_y].tolist()
                    }]
                }
            }

            return json.dumps(graph_data)

        except UnicodeDecodeError as e:
            print(f"Error reading the CSV file: {e}")

if __name__ == "__main__":
    response = main()
    print(response)
