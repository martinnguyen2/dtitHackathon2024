# dtitHackathon2024

## Inspiration
Data analysis is a topic close to us. With semantic search and GPT models all around, we aim to make data analysis accessible to everyone. Whether you're a beginner or have a strong analytical background, our platform uses natural language interactions and intuitive visualizations to help everyone understand complex datasets.

## Features Overview
- **Dataset Flexibility**: Automatically matches prompts to the most relevant dataset based on your query.  
  *Example*: "Visualize gender distribution from the supermarket dataset."  
- **Custom Data Upload**: Easily upload your own datasets (.csv files with a header row supported).  
- **Intuitive Data Visualization and Explanation**:  
  Generate beautiful charts and clear explanations designed for both professionals and non-data analysts.  
- **Predictive Modeling**: Offers an experimental SVM predictive model for relevant datasets.  
- **Voice Recognition**: Create prompts using either voice input or a text interface.  
- **Export Options**: Export graphs and analytical text in various file formats.

## How We Built It
- **Frontend**: Angular  
- **Backend**: API  
- **Data Analysis & Visualization**: Python  

## Key Advantages
- **Beginner-Friendly**: Simplifies complex data insights into easy-to-understand answers for beginners.  
- **User-Friendly Prompts**: Simply ask in plain language, and we'll understand!  
- **Smart Data Preprocessing**: Relevant data is preprocessed automatically, while raw data remains available for advanced users.  
- **Natural Language Queries**: Utilizes advanced NLP and GPT models to deliver accurate results based on your questions.  
- **Multilingual Support**: Ask questions in your preferred language! Slovak, Hungarian, or others—we've got you covered! 🌍  
- **Intelligent Prompt Filtering**: Irrelevant queries are blocked. For example, asking, *"When did Mariah Carey release an album?"* in an economic dataset won’t get in your way! 🙃  


## How to run
```
1 - Install python dependencies (pip install -r requirements.txt under analysis)
Example raw executions:
python main.py --dataset_path "..\\data\\02_un_data\\SYB66_200_202310_Employment.csv" --action explain --prompt "What is this dataset about?"
python main.py --dataset_path "..\\data\\02_un_data\\SYB66_200_202310_Employment.csv" --prompt "So what are examples of regions?" --cacheId "1"
python main.py --dataset_path "..\\data\\02_un_data\\SYB66_200_202310_Employment.csv" --action visualize "In which countries are sold products? Showcase number per each city. Make it line plot"
python main.py --dataset_path "..\\data\\02_un_data\\supermarket_sales new.csv" --action analyze --prompt "What are possible gender values in the dataset?"

2 - Add open AI api key under .env in backend.API project in format OPEN_API_KEY=...
3 - Build backend project and hit dotnet run
4 - Install Frontend packages (npm i) and run frontend -> ng serve.

You should be ready to go..
```

Images:

![Screenshot 2024-11-24 090018](https://github.com/user-attachments/assets/6fd802b0-c659-4927-bc39-fc777b12ad08)

![screencast-chat (1)](https://github.com/user-attachments/assets/7a31ee3d-b921-4343-90ba-7e723388d445)

![Screenshot 2024-11-24 111110](https://github.com/user-attachments/assets/08e9baa2-ac3c-4fd7-9943-4562126dad18)



