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
      department: {
        departmentName: "",
        academicId: null,
      },
      academics: [],
    };
  },
  mounted() {
    this.fetchDepartment();
    this.fetchAcademics();
  },
  methods: {
    async updateDepartment() {
      try {
        const response = await fetch(`http://localhost/api/departments/${this.id}`, {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            newDepartmentName: this.department.departmentName,
            newAcademicId: Number(this.department.academicId),
          }),
        });
        if (!response.ok) throw new Error("Failed to update department");
        this.$router.push("/departments/all");
      } catch (error) {
        console.error("Error updating department:", error);
      }
    },
    async fetchDepartment() {
      try {
        const response = await fetch(`http://localhost/api/departments/${this.id}`);
        if (!response.ok) throw new Error("Failed to fetch department");
        this.department = await response.json();
      } catch (error) {
        console.error("Error fetching department:", error);
      }
    },
    async fetchAcademics() {
      try {
        const response = await fetch("http://localhost/api/academics");
        if (!response.ok) throw new Error("Failed to fetch academics");
        this.academics = await response.json();
      } catch (error) {
        console.error("Error fetching academics:", error);
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
                <strong><i class="fa-solid fa-building"></i> Edit Department Data</strong>
              </h2>
              <div class="card-block">
                <form @submit.prevent="updateDepartment">
                  <!-- Department Name Input -->
                  <div class="md-form">
                    <p>Department Name</p>
                    <input
                      type="text"
                      v-model="department.departmentName"
                      name="newDepartmentName"
                      id="form-departmentname"
                      class="form-control"
                    />
                  </div>
                  <!-- Academic Dropdown -->
                  <div class="md-form">
                    <p>Select Academic</p>
                    <select
                      v-model="department.academicId"
                      name="newAcademicId"
                      id="form-academicid"
                      class="form-select"
                    >
                      <option value="" disabled>Select an academic</option>
                      <option
                        v-for="academic in academics"
                        :key="academic.academicId"
                        :value="academic.academicId"
                      >
                        {{ academic.academicName }}
                      </option>
                    </select>
                  </div>
                  <!-- Submit Button -->
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
