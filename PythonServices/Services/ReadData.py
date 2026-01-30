import os
import pandas as pd
import dotenv
import mysql.connector
from pathlib import Path

env_path = Path(__file__).resolve().parent.parent.parent / "keys.env"

dotenv.load_dotenv(env_path)

db_pass = os.getenv("MSQL_PASSWORD")

if not db_pass:
    raise Exception("ERROR: Missing key for database connection")

conn = mysql.connector.connect(
    host="localhost",
    user="root",
    password=db_pass,
    database="pokemon_admin",
)

query = "SELECT * FROM pokemon"

df = pd.read_sql(query, conn)

conn.close()

print(df)


