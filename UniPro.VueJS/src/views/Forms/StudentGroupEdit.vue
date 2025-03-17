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
      studentGroup: {
        studentGroupName: "",
        departmentId: null,
      },
      departments: [],
    };
  },
  mounted() {
    this.fetchStudentGroup();
    this.fetchDepartments();
  },
  methods: {
    async updateStudentGroup() {
      try {
        const response = await fetch(`http://localhost/api/student-groups/${this.id}`, {
          method: "PUT",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            newStudentGroupName: this.studentGroup.studentGroupName,
            newDepartmentId: Number(this.studentGroup.departmentId),
          }),
        });
        if (!response.ok) throw new Error("Failed to update student group");
        this.$router.push("/student-groups/all");
      } catch (error) {
        console.error("Error updating student group:", error);
      }
    },
    async fetchStudentGroup() {
      try {
        const response = await fetch(`http://localhost/api/student-groups/${this.id}`);
        if (!response.ok) throw new Error("Failed to fetch student group");
        this.studentGroup = await response.json();
      } catch (error) {
        console.error("Error fetching student group:", error);
      }
    },
    async fetchDepartments() {
      try {
        const response = await fetch("http://localhost/api/departments");
        if (!response.ok) throw new Error("Failed to fetch departments");
        this.departments = await response.json();
      } catch (error) {
        console.error("Error fetching departments:", error);
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
                <strong><i class="fa-solid fa-users"></i> Edit Student Group Data</strong>
              </h2>
              <div class="card-block">
                <form @submit.prevent="updateStudentGroup">
                  <div class="md-form">
                    <p>Student Group Name</p>
                    <input
                      type="text"
                      v-model="studentGroup.studentGroupName"
                      name="newStudentGroupName"
                      id="form-studentgroupname"
                      class="form-control"
                    />
                  </div>
                  <div class="md-form">
                    <p>Select Department</p>
                    <select
                      v-model="studentGroup.departmentId"
                      name="newDepartmentId"
                      id="form-departmentid"
                      class="form-select"
                    >
                      <option value="" disabled>Select a department</option>
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
      </div>
    </main>
  </div>
</template>
