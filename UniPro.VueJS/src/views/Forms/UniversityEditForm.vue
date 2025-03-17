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
            id: this.$route.params.id,
            university: {
                universityName: ''
            }
        };
    },
    mounted() {
        this.fetchUniversity();
    },
    methods: {
        async updateUniversity() {
            try {
                const response = await fetch(`http://localhost/api/universities/${this.id}`, {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ newUniversityName: this.university.universityName })
                });
                if (!response.ok) throw new Error("Failed to update university");
                this.$router.push("/universities/all");
            } catch (error) {
                console.error("Error updating university:", error);
            }
        },
        async fetchUniversity() {
            try {
                const response = await fetch(`http://localhost/api/universities/${this.id}`);
                if (!response.ok) throw new Error("Failed to fetch university");
                this.university = await response.json();
            } catch (error) {
                console.error("Error fetching university:", error);
            }
        },

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
                  <strong><i class="fa-solid fa-building-columns"></i> Edit university data</strong>
                </h2>
  
                <div class="card-block">
                  <form @submit.prevent="updateUniversity">
                    <!-- Names -->
                    <div class="md-form">
                        <p>University Name</p>
                      <input type="text" v-model="university.universityName" name="newUniversityName" id="form-uniname" class="form-control" />
                    </div>
                    <div class="text-left">
                      <button type="submit" class="btn btn-primary">Submit</button>
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

