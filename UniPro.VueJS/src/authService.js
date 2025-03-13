import * as Session from "supertokens-web-js/recipe/session";

export async function getUserInfo() {
  const session = await Session.doesSessionExist();
  alert(session);
  if (session) {
    const userId = await Session.getUserId();
    let userData = null;
    try {
      const response = await fetch(`http://localhost/api/users/${userId}`, { method: "GET" });
      if (response.ok) {
        userData = await response.json();
      } else {
        console.error("Error getting user data:", response.statusText);
      }
    } catch (error) {
      console.error("Error requesting user data:", error);
    }
    return { session, userId, userData };
  }
  return { session };
}
