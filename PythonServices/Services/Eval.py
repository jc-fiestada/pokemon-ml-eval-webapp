from Services import DataHandlers
import pandas as pd

def ModelEval(quantity: int = 150, random_state: int = 42):    
    X_train, X_test, y_train, y_test, name_train, name_test  = DataHandlers.SplitData(quantity, random_state)
    knn_prediction, log_reg_prediction, tree_prediction = DataHandlers.TrainAndTestModels(X_train, y_train, X_test)

    ModelEval: pd.DataFrame = X_test.copy()
    ModelEval[["id" ,"name"]] = name_test.copy()
    ModelEval["true_value"] = y_test.copy()
    ModelEval["knn_predicted_prediction"] = knn_prediction
    ModelEval["log_reg_prediction"] = log_reg_prediction
    ModelEval["tree_prediction"] = tree_prediction

    model_evals = {
        "model_eval" : ModelEval.to_dict(orient="records"),
        "knn_metrics" : DataHandlers.RunMetrics(y_test, knn_prediction),
        "log_reg_metrics" :  DataHandlers.RunMetrics(y_test, log_reg_prediction),
        "tree_metrics" :  DataHandlers.RunMetrics(y_test, tree_prediction)
    }

    return model_evals




