import openai
import os
import json

def raw_chat_with_gpt_without_cache(context, user_input):
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
        messages.append({"role": "assistant", "content": qa_res})

        return qa_res
    except Exception as e:
        return json.dumps({"error": str(e)})

def chat_with_gpt(context, user_input, cacheId=None):
    api_key = os.getenv("OPENAI_API_KEY")
    if not api_key:
        raise EnvironmentError("OPENAI_API_KEY environment variable not set")
    
    openai.api_key = api_key

    messages = []

    cache_file_path = "chat_cache.json"

    if cacheId:
        # Load message history from cache
        if os.path.exists(cache_file_path):
            with open(cache_file_path, "r") as cache_file:
                cache = json.load(cache_file)
                messages = cache.get(cacheId, [])
    else:
        # Create a new cacheId
        if os.path.exists(cache_file_path):
            with open(cache_file_path, "r") as cache_file:
                cache = json.load(cache_file)
            cacheId = str(len(cache) + 1)
        else:
            cache = {}
            cacheId = "1"


    messages.append({"role": "system", "content": f"Relevant context: {context}"})
    messages.append({"role": "user", "content": user_input})

    try:
        response = openai.chat.completions.create(
            model="gpt-4",
            messages=messages,
            temperature=0.5,
        )

        qa_res = response.choices[0].message.content.strip()
        messages.append({"role": "assistant", "content": qa_res})

        # Save message history to cache
        if os.path.exists(cache_file_path):
            with open(cache_file_path, "r") as cache_file:
                cache = json.load(cache_file)
        else:
            cache = {}

        cache[cacheId] = messages

        with open(cache_file_path, "w") as cache_file:
            json.dump(cache, cache_file)

        
        if qa_res.lower().startswith("error"):
            return json.dumps({"error": response.text_output})

        return json.dumps({"text_output": qa_res, "cacheId": cacheId})

    except Exception as e:
        return json.dumps({"error": str(e)})