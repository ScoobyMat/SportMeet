export const errorMessages = {
    "Invalid email or password": "Nieprawidłowy adres e-mail lub hasło",
    "User not found": "Użytkownik nie znaleziony",
    "A user with this email address already exists" : "Istnieje już konto o danym adresie e-mail."
  };
  
  export function translateErrorMessage(message) {
    return errorMessages[message] || "Wystąpił błąd. Spróbuj ponownie później.";
  }