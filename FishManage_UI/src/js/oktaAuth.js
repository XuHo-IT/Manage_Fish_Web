import { OktaAuth } from "@okta/okta-auth-js";

const authClient = new OktaAuth({
  issuer: "https://dev-51113269.okta.com/oauth2/default",
  clientId: "0oanvas387G3ON4jB5d7",
  redirectUri: "http://localhost:5173/",
  responseType: ["id_token", "token"],
});
