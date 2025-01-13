import { reactive } from "vue";

const toasts = reactive([]);

let toastId = 0;

export const addToast = (title, message) => {
  const id = ++toastId;
  toasts.push({ id, title, message });

  setTimeout(() => {
    const index = toasts.findIndex((toast) => toast.id === id);
    if (index !== -1) {
      toasts.splice(index, 1);
    }
  }, 5000);
};

export const useToasts = () => toasts;
