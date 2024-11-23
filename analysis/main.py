import argparse
import pandas as pd
from dotenv import load_dotenv
from chatbot.OpenAiChat import chat_with_gpt

def main():
    load_dotenv()
    parser = argparse.ArgumentParser(description="Process some parameters.")
    parser.add_argument('--action', type=str, choices=['explain', 'visualize'], help='Action to perform')
    parser.add_argument('--dataset_path', type=str, help='Path to the dataset')
    parser.add_argument('--prompt', type=str, help='Prompt for the discussion')
    parser.add_argument('--cacheId', type=str, help='Cache ID for message history', default=None)
    
    args = parser.parse_args()
    
    if args.cacheId:
        # Continue the conversation using the provided cacheId
        context = ""
        user_input = args.prompt
        response = chat_with_gpt(context, user_input, args.cacheId)
        return response
    elif args.action == 'explain':
        try:
            df = pd.read_csv(args.dataset_path, encoding='latin1')
            context = "Explain what this dataset is about "
            df_head = df.head(100)
            csv_content = df_head.to_csv(index=False)
            user_input = f"{csv_content}"
            response = chat_with_gpt(context, user_input)
            return response
        except UnicodeDecodeError as e:
            print(f"Error reading the CSV file: {e}")
            return None

if __name__ == "__main__":
    response = main()
    print(response)