from Services import ReadData
from sklearn.tree import DecisionTreeClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.neighbors import KNeighborsClassifier
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.pipeline import Pipeline
from pathlib import Path
import pandas as pd
from sklearn.metrics import f1_score, accuracy_score, recall_score, precision_score

def TrainAndTestModels(X_train_data : pd.DataFrame, y_train_data : pd.Series, X_test_data : pd.DataFrame):
    ModelPipelines = {
        "KNN" : Pipeline([
            ("knnScaler", StandardScaler()),
            ("knnModel", KNeighborsClassifier())
        ]),

        "LogisticRegression" : Pipeline([
            ("logScaler", StandardScaler()),
            ("logModel", LogisticRegression())
        ]),

        "DecisionTree" : Pipeline([
            ("treeModel", DecisionTreeClassifier())
        ])
    }

    ModelPipelines["KNN"].fit(X_train_data, y_train_data)
    ModelPipelines["LogisticRegression"].fit(X_train_data, y_train_data)
    ModelPipelines["DecisionTree"].fit(X_train_data, y_train_data)

    knn_prediction = ModelPipelines["KNN"].predict(X_test_data)
    log_reg_prediction = ModelPipelines["LogisticRegression"].predict(X_test_data)
    tree_prediction = ModelPipelines["DecisionTree"].predict(X_test_data)

    return knn_prediction, log_reg_prediction, tree_prediction


def SplitData(quantity: int = 150, random_state: int = 42):
    env_path = Path(__file__).resolve().parent.parent.parent / "keys.env"
    pokemon_df = ReadData.ReadPokemonDataset(env_path, quantity)

    X = pokemon_df[["health", "defense", "weight", "attack", "speed", "height", "special_attack", "special_defense"]]
    y = pokemon_df["type"]
    identifier = pokemon_df[["id" ,"name"]]

    X_train, X_test, y_train, y_test, identifier_train, identifier_test = train_test_split(X, y, identifier, random_state=random_state, test_size=.20)

    return X_train, X_test, y_train, y_test, identifier_train, identifier_test

def RunMetrics(y_true, y_prediction):
    accuracy = accuracy_score(y_true, y_prediction)
    recall = recall_score(y_true, y_prediction, average="weighted", zero_division=0)
    precision = precision_score(y_true, y_prediction, average="weighted", zero_division=0)
    f1 = f1_score(y_true, y_prediction, average="weighted")

    return {
        "f1_score" : f1,
        "accuracy_score" : accuracy,
        "precision_score" : precision,
        "recall_score" : recall 
    }



