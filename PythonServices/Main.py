from fastapi import FastAPI
from Services.Eval import ModelEval
from Models.PokemonClass import UserRequest



app = FastAPI(title="Model Eval Api")

@app.post("/predict/pokemon-type")
def predict(user: UserRequest):
    return ModelEval(user.quantity, user.randomState)

