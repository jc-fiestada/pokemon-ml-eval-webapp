import ReadData
from sklearn.tree import DecisionTreeClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.neighbors import KNeighborsClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import f1_score, accuracy_score, precision_score, recall_score
from sklearn.preprocessing import StandardScaler
from sklearn.pipeline import Pipeline
from pathlib import Path
import pandas as pd


def TrainAndTestModels(quantity: int = 150, random_state: int = 42):
    env_path = Path(__file__).resolve().parent.parent.parent / "keys.env"

    ModelPipelines = {
        "KNN" : Pipeline([
            ("knnScaler", StandardScaler()),
            ("knnModel", KNeighborsClassifier())
        ]),

        "LogisticRegression" : ([
            ("logScaler", StandardScaler()),
            ("logModel", LogisticRegression())
        ]),

        "DecisionTree" : ([
            ("treeScaler", StandardScaler()),
            ("treeModel", DecisionTreeClassifier())
        ])
    }

    pokemon_df = ReadData.ReadPokemonDataset(env_path, quantity)

    X = pokemon_df[["health", "defense", "weight", "attack", "speed", "height", "special_attack", "special_defense"]]
    y = pokemon_df["type"]
    names = pokemon_df["names"]

    X_train, X_test, y_train, y_test, name_train, name_test = train_test_split(X, y, names, random_state=random_state, test_size=.20)



    print(pokemon_df)