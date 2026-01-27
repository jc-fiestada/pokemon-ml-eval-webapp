export function showToast(message: string, duration: number = 3000): void {
    const container = document.getElementById("toast-container");
    if (!container) return;

    const toast = document.createElement("div");
    toast.className = "toast";
    toast.textContent = message;

    container.appendChild(toast);

    setTimeout(() => {
        toast.classList.add("exit");

        toast.addEventListener("animationend", () => {
            toast.remove();
        });
    }, duration);
}
