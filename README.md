# dtitHackathon2024

pip install virtualenv
virtualenv hackathon
.\hackathon\Scripts\activate 

python main.py --dataset_path "D:\\dtitHackathon2024\\data\\02_un_data\\SYB66_200_202310_Employment.csv" --action explain

python main.py --dataset_path "D:\\dtitHackathon2024\\data\\02_un_data\\SYB66_200_202310_Employment.csv" --prompt "So what are examples of regions?" --cacheId "1"

--
pip freeze > requirements.txt