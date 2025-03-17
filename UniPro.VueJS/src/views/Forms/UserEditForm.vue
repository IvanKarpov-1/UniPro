<script>
    import Header from '../../components/navbar/Header.vue';
    import { defineComponent } from "vue";
    import * as Session from "supertokens-web-js/recipe/session";
    import { getUserInfo } from "../../authService";
    export default defineComponent({
    components: {
        Header,
    },
    data() {
        return {
            session: false,
            userId: "",
            payload: null,
            userData: {},
            token: "",
        };
    },
    async mounted() {
        getUserInfo().then((data) => {
            this.session = data.session;
            this.userId = data.userId;
            this.userData = data.userData;
        });
    },
    methods: {
        async updateUserData() {
            try {
                const response = await fetch(`http://localhost/api/users/${this.userId}`, {
                    method: "PUT",
                    body: JSON.stringify(this.userData),
                });

                if (!response.ok) {
                    throw new Error("Failed to update user data");
                }

                const result = await response.json();
                console.log("User data updated successfully", result);
            } catch (error) {
                console.error("Error updating user data:", error);
            }
        }
    },
    });
</script>
<template>
    <Header />
    <div class="container">
      <main class="freeBird">
        <div class="container">
          <div class="row">
            <div class="col-md-7 mx-auto">
              <div class="jumbotron p-5">
                <h2 class="h2-responsive">
                  <strong>Edit user data</strong>
                </h2>
                <p>You can change your data here <br>(Personal Data can be changed only by administration)</p>
  
                <div class="card-block">
                  <form action="#">
                    <!-- Names -->
                    <h5 class="h5-responsive"><i class="fa fa-user prefix"></i> Personal Data</h5>
                    <div class="md-form">
                      <input type="text" v-model="userData.firstName" name="firstName" id="form-fname" class="mdc-text-field__input" 
                      placeholder="Placeholder text" aria-label="Label" readonly/>
                    </div>
                    <div class="md-form">
                        <input type="text" v-model="userData.lastName" name="lastName" id="form-lname" class="form-control" readonly/>
                     
                    </div>
                    <div class="md-form">
                        <input type="text" v-model="userData.patronymic" name="patronymic" id="form-patr" class="form-control" readonly/>
                    </div>
                    
                    <!-- Phone -->
                    <h5 class="h5-responsive"><i class="fa-solid fa-phone"></i> Phone number</h5>
                    <div class="md-form">
                      <input type="text" v-model="userData.phoneNumber" name="phoneNumber" id="form-phone" class="form-control validate" />
                      <label for="form-phone" data-error="please put correct phone" data-success="great!"></label>
                    </div>
                    <div class="text-left">
                      <button class="btn btn-primary" @click.prevent="updateUserData">Submit</button>
                    </div>
                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
</template>

<style scoped>


.freeBird {
    margin-top: 20px;  
}

.small-text {
    font-size:12px;
}
</style>