<script>
import { defineComponent } from 'vue';
import Header from '@/components/navbar/Header.vue';

export default defineComponent({
  components: {
    Header,
  },
  data() {
    return {
      departments: [],
      selectedDepartmentId: null,
      studentGroupName: '',
    };
  },
  mounted() {
    this.fetchDepartments();
  },
  methods: {
    async fetchDepartments() {
      try {
        const response = await fetch("http://localhost/api/departments");
        if (!response.ok) {
          throw new Error("Failed to fetch departments");
        }
        this.departments = await response.json();
      } catch (error) {
        console.error("Error fetching departments:", error);
      }
    },
    async submitStudentGroup() {
      // form validation
      if (!this.studentGroupName.trim() || !this.selectedDepartmentId) {
        alert("Please fill all required fields.");
        return;
      }
      try {
        const payload = {
          departmentId: Number(this.selectedDepartmentId),
          studentGroupName: this.studentGroupName,
        };
        const response = await fetch("http://localhost/api/student-groups", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(payload),
        });
        if (!response.ok) {
          throw new Error("Failed to create student group");
        }
        await response.json();
        alert("Student group created successfully!");
        this.studentGroupName = "";
        this.selectedDepartmentId = null;
      } catch (error) {
        console.error("Error creating student group:", error);
        alert("Error creating student group");
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
              <strong>Create Student Group</strong>
            </h2>
            <p>Fill the form below to create a new Student Group</p>
            <div class="card-block">
              <form @submit.prevent="submitStudentGroup">
                <div class="md-form">
                  <input
                    type="text"
                    v-model="studentGroupName"
                    name="studentGroupName"
                    id="form-studentgroup"
                    class="form-control"
                    required
                  />
                  <label for="form-studentgroup">Student Group Name</label>
                </div>
                <div class="form-controll">
                  <label for="form-department">Department</label>
                  <select
                    v-model="selectedDepartmentId"
                    id="form-department"
                    class="form-select"
                    required
                  >
                    <option value="" disabled selected>Select a department</option>
                    <option
                      v-for="department in departments"
                      :key="department.departmentId"
                      :value="department.departmentId"
                    >
                      {{ department.departmentName }}
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
