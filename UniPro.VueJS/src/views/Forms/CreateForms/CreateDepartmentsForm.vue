<script>
import { defineComponent } from 'vue';
import Header from '@/components/navbar/Header.vue';

export default defineComponent({
  components: {
    Header,
  },
  data() {
    return {
      academics: [],
      departmentName: '',
      selectedAcademicId: null,
    };
  },
  mounted() {
    this.fetchAcademics();
  },
  methods: {
    async fetchAcademics() {
      try {
        const response = await fetch("http://localhost/api/academics");
        if (!response.ok) {
          throw new Error("Failed to fetch academics");
        }
        this.academics = await response.json();
      } catch (error) {
        console.error("Error fetching academics:", error);
      }
    },
    async submitDepartment() {
      // validation
      if (!this.departmentName.trim() || !this.selectedAcademicId) {
        alert("Please fill all required fields.");
        return;
      }
      try {
        const payload = {
          academicId: Number(this.selectedAcademicId),
          departmentName: this.departmentName,
        };
        const response = await fetch("http://localhost/api/departments", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(payload),
        });
        if (!response.ok) {
          throw new Error("Failed to create department");
        }
        const result = await response.json();
        alert("Department created successfully!");
        this.departmentName = "";
        this.selectedAcademicId = null;
      } catch (error) {
        console.error("Error creating department:", error);
        alert("Error creating department");
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
              <strong>Create Department</strong>
            </h2>
            <p>Fill the form below to create a new Department</p>
            <div class="card-block">
              <form @submit.prevent="submitDepartment">
                <div class="md-form">
                  <input
                    type="text"
                    v-model="departmentName"
                    name="departmentName"
                    id="form-department"
                    class="form-control"
                    required
                  />
                  <label for="form-department">Department Name</label>
                </div>
                <div class="form-controll">
                  <label for="form-academic">Academic</label>
                  <select
                    v-model="selectedAcademicId"
                    id="form-academic"
                    class="form-select"
                    required
                  >
                    <option value="" disabled selected>Select an academic</option>
                    <option
                      v-for="academic in academics"
                      :key="academic.academicId"
                      :value="academic.academicId"
                    >
                      {{ academic.academicName }}
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
