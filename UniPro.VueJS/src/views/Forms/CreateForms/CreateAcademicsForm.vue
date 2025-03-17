<script>
import { defineComponent } from 'vue';
import Header from '@/components/navbar/Header.vue';

export default defineComponent({
  components: {
    Header,
  },
  data() {
    return {
      universities: [],
      academicName: null,

    };
  },
  mounted() {
    this.fetchUniversities();
  },
  methods: {
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
    async submitAcademic() {
      // form validation 
      if (
        !this.academicName.trim() ||
        !this.selectedUniversityId
      ) {
        alert("Please fill all required fields.");
        return;
      }
      try {
        const payload = {
          universityId: Number(this.selectedUniversityId),
          academicName: this.academicName,
        };
        console.log(payload);
        const response = await fetch("http://localhost/api/academics", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(payload),
        });
        if (!response.ok) {
          throw new Error("Failed to create academic");
        }
        const result = await response.json();
        alert("Academic created successfully!");
        this.academicName = "";
        this.selectedUniversityId = null;
      } catch (error) {
        console.error("Error creating academic:", error);
        alert("Error creating academic");
      }
    },
  },
});
</script>
<template>
    <Header />
  <div class="container">
    <main class="freeBird">
      <div class="row">
        <div class="col-md-7 mx-auto">
          <div class="jumbotron p-5">
            <h2 class="h2-responsive">
              <strong>Create Academic</strong>
            </h2>
            <p>Fill the form below to create a new Acadmic</p>
            <div class="card-block">
              <form @submit.prevent="submitAcademic">
                <div class="md-form">
                  <input type="text" v-model="academicName" name="academicName" id="form-academic" class="form-control" required />
                  <label for="form-academic">Academic name </label>
                </div>
                <div class="form-controll">
                  <label for="form-university">University</label>
                  <select v-model="selectedUniversityId" id="form-university" class="form-select" required>
                    <option value="" disabled selected>Select a university</option>
                    <option v-for="uni in universities" :value="uni.universityId" :key="uni.universityId">
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
    </main>
  </div>
</template>