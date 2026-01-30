import os
import pandas as pd
import dotenv
import mysql.connector

def ReadPokemonDataset(env_path: str, quantity: int = 150):
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

    query = "SELECT * FROM pokemon LIMIT %s"
    
    
    df = pd.read_sql(query, conn, params=(quantity,))
    conn.close()

    return df