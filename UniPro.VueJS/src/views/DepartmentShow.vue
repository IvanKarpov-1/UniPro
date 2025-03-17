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
    async deleteDepartment(id) {
      if (!confirm("Are you sure you want to delete this department?")) {
        return;
      }
      try {
        const response = await fetch(`http://localhost/api/departments/${id}`, {
          method: "DELETE",
        });
        if (!response.ok) {
          throw new Error("Failed to delete department");
        }
        this.departments = this.departments.filter(department => department.departmentId !== id);
        window.location.reload();
      } catch (error) {
        console.error("Error deleting department:", error);
      }
    },
    toEdit(id) {
      this.$router.push({ path: `/department/${id}` });
    }
  },
});
</script>

<template>
  <Header />
  <div class="container mt-4">
    <h2>Departments List</h2>
    <table class="table table-bordered">
      <thead>
        <tr>
          <th>ID</th>
          <th>Department Name</th>
          <th>Academic ID</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="department in departments" :key="department.departmentId">
          <td>{{ department.departmentId }}</td>
          <td>{{ department.departmentName }}</td>
          <td><a :href="'/academic/' + department.academicId">{{ department.academicId }}</a></td>
          <td>
            <button class="btn btn-danger btn-sm" @click="deleteDepartment(department.departmentId)">
              <i class="fa-solid fa-trash-can"></i>
            </button>
            <button class="btn btn-warning btn-sm ml-2" @click="toEdit(department.departmentId)">
              <i class="fa-solid fa-pen"></i>
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
