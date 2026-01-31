import DataHandlers
import pandas as pd

def ModelEval():    
    X_train, X_test, y_train, y_test, name_train, name_test  = DataHandlers.SplitData(10)
    knn_prediction, log_reg_prediction, tree_prediction = DataHandlers.TrainAndTestModels(X_train, y_train, X_test)

    KNN: pd.DataFrame = X_test.copy()
    KNN[["id" ,"name"]] = name_test.copy()
    KNN["true_value"] = y_test.copy()
    KNN["predicted_value"] = knn_prediction
    
    LogReg: pd.DataFrame = X_test.copy()
    LogReg[["id", "name"]] = name_test.copy()
    LogReg["true_value"] = y_test.copy()
    LogReg["predicted_value"] = log_reg_prediction

    Tree: pd.DataFrame = X_test.copy()
    Tree[["id", "name"]] = name_test.copy()
    Tree["true_value"] = y_test.copy()
    Tree["predicted_value"] = tree_prediction

    print(LogReg)
    print(DataHandlers.RunMetrics(y_test, log_reg_prediction))

    model_evals = {
        "KNN" : KNN.to_dict(orient="records"),
        "KNN_Metrics" : DataHandlers.RunMetrics(y_test, knn_prediction),
        "LogReg" : LogReg.to_dict(orient="records"),
        "LogReg_Metrics" :  DataHandlers.RunMetrics(y_test, log_reg_prediction),
        "Tree" : Tree.to_dict(orient="records"),
        "Tree_Metrics" :  DataHandlers.RunMetrics(y_test, tree_prediction)
    }

    return model_evals




