import { showToast } from "./animation.js";
const runEvalButton = document.getElementById("runEval");
const loadPokemonButton = document.getElementById("loadPokemon");
const randomState = document.getElementById("randomState");
const sampleFreq = document.getElementById("sampleFreq");
const pokemonList = document.getElementById("pokemon-list");
const knnAccuracy = document.getElementById("knn-acc");
const knnPrecision = document.getElementById("knn-pre");
const knnRecall = document.getElementById("knn-rec");
const knnF1 = document.getElementById("knn-f1");
const logRegAccuracy = document.getElementById("lr-acc");
const logRegPrecision = document.getElementById("lr-pre");
const logRegRecall = document.getElementById("lr-rec");
const LogRegF1 = document.getElementById("lr-f1");
const treeAccuracy = document.getElementById("tree-acc");
const treePrecision = document.getElementById("tree-pre");
const treeRecall = document.getElementById("tree-rec");
const treeF1 = document.getElementById("tree-f1");
addEventListener("DOMContentLoaded", async () => {
    randomState.value = "1";
    sampleFreq.value = "7";
    const response = await fetch("/verify/page-access", {
        headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
    });
    if (response.status === 401) {
        window.location.href = "unauthorized.html";
        return;
    }
    if (!response.ok) {
        window.location.href = "error.html";
        return;
    }
    showToast("Welcome Admin!");
});
runEvalButton.addEventListener("click", async () => {
    runEvalButton.disabled = true;
    pokemonList.innerHTML = "";
    const response = await fetch("/predict/pokemon", {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            quantity: parseInt(sampleFreq.value),
            randomState: parseInt(randomState.value)
        })
    });
    if (response.status === 400) {
        showToast("Invalid Evaluation Input Value/s");
        runEvalButton.disabled = false;
        return;
    }
    if (response.status === 422 || response.status === 500) {
        const message = await response.text();
        showToast(message);
        runEvalButton.disabled = false;
        return;
    }
    if (response.status === 401) {
        window.location.href = "unauthorized.html";
        runEvalButton.disabled = false;
        return;
    }
    showToast("Processing...");
    const data = await response.json();
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
    runEvalButton.disabled = false;
});
loadPokemonButton.addEventListener("click", async () => {
    loadPokemonButton.disabled = true;
    showToast("Processing...");
    const response = await fetch("/store/db/pokemon", {
        method: "GET",
        headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
    });
    if (response.status === 401) {
        window.location.href = "unauthorized.html";
        runEvalButton.disabled = false;
        return;
    }
    if (response.status === 409 || response.status === 500) {
        const message = await response.text();
        showToast(message);
        loadPokemonButton.disabled = false;
        return;
    }
    showToast("Pokemon has been successfully stored in the db");
    loadPokemonButton.disabled = false;
});
//# sourceMappingURL=admin.js.map