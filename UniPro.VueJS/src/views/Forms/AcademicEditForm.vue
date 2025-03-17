<script>
import Header from "../../components/navbar/Header.vue";
import { defineComponent } from "vue";

export default defineComponent({
  components: {
    Header,
  },
  data() {
    return {
      id: this.$route.params.id,
      academic: {
        academicName: "",
        universityId: null,
      },
      universities: [],
    };
  },
  mounted() {
    this.fetchAcademic();
    this.fetchUniversities();
  },
  methods: {
    async updateAcademic() {
      try {
        const response = await fetch(`http://localhost/api/academics/${this.id}`, {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            newAcademicName: this.academic.academicName,
            newUniversityId: Number(this.academic.universityId),
          }),
        });
        if (!response.ok)
          throw new Error("Failed to update academic");
        this.$router.push("/academics/all");
      } catch (error) {
        console.error("Error updating academic:", error);
      }
    },
    async fetchAcademic() {
      try {
        const response = await fetch(`http://localhost/api/academics/${this.id}`);
        if (!response.ok)
          throw new Error("Failed to fetch academic");
        this.academic = await response.json();
      } catch (error) {
        console.error("Error fetching academic:", error);
      }
    },
    async fetchUniversities() {
      try {
        const response = await fetch("http://localhost/api/universities");
        if (!response.ok) {
          throw new Error("Failed to fetch universities");
        }
        this.universities = await response.json();
      } catch (error) {
        console.error("Error fetching universities:", error);
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
                <strong><i class="fa-solid fa-school"></i> Edit Academic Data</strong>
              </h2>
              <div class="card-block">
                <form @submit.prevent="updateAcademic">
                  <div class="md-form">
                    <p>Academic Name</p>
                    <input
                      type="text"
                      v-model="academic.academicName"
                      name="newAcademicName"
                      id="form-academicname"
                      class="form-control"
                    />
                  </div>
                  <div class="md-form">
                    <p>Select University</p>
                    <select
                      v-model="academic.universityId"
                      name="newUniversityId"
                      id="form-universityid"
                      class="form-select"
                    >
                      <option value="" disabled>Select a university</option>
                      <option
                        v-for="uni in universities"
                        :key="uni.universityId"
                        :value="uni.universityId"
                      >
                        {{ uni.universityName }}
                      </option>
                    </select>
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
