import openai
import os

def chat_with_gpt(context, user_input):
    api_key = os.getenv("OPENAI_API_KEY")
    if not api_key:
        raise EnvironmentError("OPENAI_API_KEY environment variable not set")
    
    openai.api_key = api_key

    messages = []
    messages.append({"role": "system", "content": f"Relevant context: {context}"})
    messages.append({"role": "user", "content": user_input})

    try:
        response = openai.chat.completions.create(
            model="gpt-4",
            messages=messages,
            temperature=0.5,
        )

        qa_res = response.choices[0].message.content.strip()
        print(f"QA Bot: {qa_res}")
        messages.append({"role": "assistant", "content": qa_res})

    except openai.error.OpenAIError as e:
        print(f"Error in generating response: {e}")