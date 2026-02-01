export interface PokemonEval{
    health: number;
    defense: number;
    weight: number;
    attack: number;
    speed: number;
    height: number;
    special_attack: number;
    special_defense: number;
    id: number;
    name: string;
    true_value: string;
    knn_prediction: string;
    log_reg_prediction: string;
    tree_prediction: string;
}

export interface Metrics{
    f1_score: number;
    accuracy_score: number;
    precision_score: number;
    recall_score: number;
}

export interface PokemonEvalResponse{
    model_eval: PokemonEval[];
    knn_metrics: Metrics;
    log_reg_metrics: Metrics;
    tree_metrics: Metrics;
}