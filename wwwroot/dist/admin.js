import { showToast } from "./animation.js";
addEventListener("DOMContentLoaded", async () => {
    const response = await fetch("/verify/page-access", {
        headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
    });
    // ill add it later
    if (response.status === 401) {
        // redirect to 401 page
        return;
    }
    if (!response.ok) {
        // redirect to error page
        return;
    }
    showToast("Welcome Admin!");
});
//# sourceMappingURL=admin.js.map