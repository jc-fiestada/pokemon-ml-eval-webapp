import { showToast } from "./animation.js";
import { PokemonEvalResponse } from "./interfaces/pokemonEval.js";

const runEvalButton = <HTMLButtonElement>document.getElementById("runEval");
const randomState = <HTMLInputElement>document.getElementById("randomState");
const sampleFreq = <HTMLInputElement>document.getElementById("sampleFreq");

randomState.value = "0"
randomState.value = "0"

const pokemonList = <HTMLDivElement>document.getElementById("pokemon-list");

const accuracyDisplay = <HTMLElement>document.getElementById("accuracy-score");
const precisionDisplay = <HTMLElement>document.getElementById("precision-score");
const recallDisplay = <HTMLElement>document.getElementById("recall-score");
const f1Display = <HTMLElement>document.getElementById("f1-score");

addEventListener("DOMContentLoaded", async () => {
    const response = await fetch("/verify/page-access", {
        headers: {"Authorization" : `Bearer ${localStorage.getItem("token")}`}
    });

    if (response.status === 401){
        window.location.href = "unauthorized.html";
        return;
    }

    if (!response.ok){
        window.location.href = "error.html";
        return;
    }
    showToast("Welcome Admin!");
});

runEvalButton.addEventListener("click", async () => {
    const response = await fetch ("/predict/pokemon", {
        method : "POST",
        headers : {
            "Authorization" : `Bearer ${localStorage.getItem("token")}`,
            "Content-Type" : "application/json"
        },
        body : JSON.stringify({
            quantity : parseInt(sampleFreq.value),
            randomState : parseInt(randomState.value)
    })});

    if (response.status === 400){
        showToast("Invalid Evaluation Input Value/s");
        return;
    }

    if (response.status === 401){
        window.location.href = "unauthorized.html";
        return;
    }

    const data : PokemonEvalResponse = await response.json();

    debugger;

    data.model_eval.forEach(element => {
        const container = document.createElement("div");
        container.innerHTML = `
            <div class="pokemon-row">
                <div class="row-left">
                    <span class="pid">#${element.id}</span>
                    <span class="pname">${element.name}</span>
                </div>

                <div class="row-mid">
                    <span>H: ${element.height}</span>
                    <span>W: ${element.weight}</span>
                    <span>HP: ${element.health}</span>
                    <span>ATK: ${element.attack}</span>
                    <span>DEF: ${element.defense}</span>
                    <span>SP: ${element.special_attack}</span>
                    <span>SD: ${element.special_defense}</span>
                    <span>SPD: ${element.speed}</span>
                </div>

                <div class="row-right">
                    <span class="true-type">True: ${element.true_value}</span>
                    <span>KNN: ${element.knn_prediction}</span>
                    <span>LogReg: ${element.log_reg_prediction}</span>
                    <span>Tree: ${element.tree_prediction}</span>
                </div>
            </div>
        `;

        pokemonList.append(container);
    });

});