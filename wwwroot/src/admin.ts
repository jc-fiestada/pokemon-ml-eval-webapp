import { showToast } from "./animation.js";
import { PokemonEvalResponse } from "./interfaces/pokemonEval.js";

const runEvalButton = <HTMLButtonElement>document.getElementById("runEval");
const randomState = <HTMLInputElement>document.getElementById("randomState");
const sampleFreq = <HTMLInputElement>document.getElementById("sampleFreq");

randomState.value = "0";
randomState.value = "0";

const pokemonList = <HTMLDivElement>document.getElementById("pokemon-list");

const knnAccuracy =<HTMLTableCellElement>document.getElementById("knn-acc");
const knnPrecision =<HTMLTableCellElement>document.getElementById("knn-pre");
const knnRecall =<HTMLTableCellElement>document.getElementById("knn-rec");
const knnF1 =<HTMLTableCellElement>document.getElementById("knn-f1");

const logRegAccuracy =<HTMLTableCellElement>document.getElementById("lr-acc");
const logRegPrecision =<HTMLTableCellElement>document.getElementById("lr-pre");
const logRegRecall =<HTMLTableCellElement>document.getElementById("lr-rec");
const LogRegF1 =<HTMLTableCellElement>document.getElementById("lr-f1");

const treeAccuracy =<HTMLTableCellElement>document.getElementById("tree-acc");
const treePrecision =<HTMLTableCellElement>document.getElementById("tree-pre");
const treeRecall =<HTMLTableCellElement>document.getElementById("tree-rec");
const treeF1 =<HTMLTableCellElement>document.getElementById("tree-f1");

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
    pokemonList.innerHTML = "";

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

    if (response.status === 422){
        const message = await response.text();
        showToast(message);
        return;
    }

    if (response.status === 401){
        window.location.href = "unauthorized.html";
        return;
    }

    showToast("Processing...");

    const data : PokemonEvalResponse = await response.json();

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

    knnAccuracy.innerText = data.knn_metrics.accuracy_score.toString();
    knnPrecision.innerText = data.knn_metrics.precision_score.toString();
    knnRecall.innerText = data.knn_metrics.recall_score.toString();
    knnF1.innerText = data.knn_metrics.f1_score.toString();

    logRegAccuracy.innerText = data.log_reg_metrics.accuracy_score.toString();
    logRegPrecision.innerText = data.log_reg_metrics.precision_score.toString();
    logRegRecall.innerText = data.log_reg_metrics.recall_score.toString();
    LogRegF1.innerText = data.log_reg_metrics.f1_score.toString();

    treeAccuracy.innerText = data.tree_metrics.accuracy_score.toString();
    treePrecision.innerText = data.tree_metrics.precision_score.toString();
    treeRecall.innerText = data.tree_metrics.recall_score.toString();
    treeF1.innerText = data.tree_metrics.f1_score.toString();

});