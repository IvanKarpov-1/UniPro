<script>
import { defineComponent } from 'vue';
import Header from '@/components/navbar/Header.vue';

export default defineComponent({
  components: {
    Header,
  },
  data() {
    return {
      studentGroups: [],
    };
  },
  mounted() {
    this.fetchStudentGroups();
  },
  methods: {
    async fetchStudentGroups() {
      try {
        const response = await fetch("http://localhost/api/student-groups");
        if (!response.ok) {
          throw new Error("Failed to fetch student groups");
        }
        this.studentGroups = await response.json();
        console.log(this.studentGroups);
      } catch (error) {
        console.error("Error fetching student groups:", error);
      }
    },
    async deleteStudentGroup(id) {
      if (!confirm("Are you sure you want to delete this student group?")) {
        return;
      }
      try {
        const response = await fetch(`http://localhost/api/student-groups/${id}`, {
          method: "DELETE",
        });
        if (!response.ok) {
          throw new Error("Failed to delete student group");
        }
        this.studentGroups = this.studentGroups.filter(group => group.studentGroupId !== id);
        window.location.reload();
      } catch (error) {
        console.error("Error deleting student group:", error);
      }
    },
    toEdit(id) {
      this.$router.push({ path: `/student-group/${id}` });
    }
  },
});
</script>

<template>
  <Header />
  <div class="container mt-4">
    <h2>Student Groups List</h2>
    <table class="table table-bordered">
      <thead>
        <tr>
          <th>ID</th>
          <th>Student Group Name</th>
          <th>Department ID</th>
          <th>Students</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="group in studentGroups" :key="group.studentGroupId">
          <td>{{ group.studentGroupId }}</td>
          <td>{{ group.studentGroupName }}</td>
          <td>{{ group.departmentId }}</td>
          <td>
            <div v-if="group.students && group.students.length">
              <ul class="list-unstyled">
                <li v-for="student in group.students" :key="student.studentId">
                  {{ student.studentId }}
                </li>
              </ul>
            </div>
            <div v-else>
              No students in this group.
            </div>
          </td>
          <td>
            <button class="btn btn-danger btn-sm" @click="deleteStudentGroup(group.studentGroupId)">
              <i class="fa-solid fa-trash-can"></i>
            </button>
            <button class="btn btn-warning btn-sm ml-2" @click="toEdit(group.studentGroupId)">
              <i class="fa-solid fa-pen"></i>
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
