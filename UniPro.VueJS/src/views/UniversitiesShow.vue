<script>
import { defineComponent } from 'vue';
import Header from '@/components/navbar/Header.vue';
import { getUserInfo } from "@/authService";
import * as Session from "supertokens-web-js/recipe/session";

export default defineComponent({
    components: {
        Header,
    },
    data() {
        return {
            universities: [],
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
        async deleteUniversity(id) {
            if (!confirm("Are you sure you want to delete this university?")) {
                return;
            }
            try {
                const response = await fetch(`http://localhost/api/universities/${id}`, {
                    method: "DELETE",
                });
                if (!response.ok) {
                    throw new Error("Failed to delete university");
                }
                this.universities = this.universities.filter(university => university.id !== id);
                window.location.reload();
            } catch (error) {
                console.error("Error deleting university:", error);
            }
        },
        toEdit(id){
            this.$router.push({ path: `/university/${id}` });
        }
    },
});
</script>

<template>
    <Header />
    <div class="container mt-4">
        <h2>Universities List</h2>
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="university in universities" :key="university.id">
                    <td>{{ university.universityId }}</td>
                    <td>{{ university.universityName }}</td>
                    <td>
                        <button class="btn btn-danger btn-sm" @click="deleteUniversity(university.universityId)"><i class="fa-solid fa-trash-can"></i></button>
                        <button class="btn btn-warning btn-sm ml-2" @click="toEdit(university.universityId)"><i class="fa-solid fa-pen"></i></button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>
