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
            academics: [],
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
        async deleteAcademic(id) {
            if (!confirm("Are you sure you want to delete this academic?")) {
                return;
            }
            try {
                const response = await fetch(`http://localhost/api/academics/${id}`, {
                    method: "DELETE",
                });
                if (!response.ok) {
                    throw new Error("Failed to delete academic");
                }
                this.academics = this.academics.filter(academic => academic.id !== id);
                window.location.reload();
            } catch (error) {
                console.error("Error deleting university:", error);
            }
        },
        toEdit(id){
            this.$router.push({ path: `/academic/${id}` });
        }
    },
});
</script>

<template>
    <Header />
    <div class="container mt-4">
        <h2>Academics List</h2>
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>University ID</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="academic in academics" :key="academic.id">
                    <td>{{ academic.academicId }}</td>
                    <td>{{ academic.academicName }}</td>
                    <td><a :href="'/university/' + academic.universityId">{{ academic.universityId }}</a></td>
                    <td>
                        <button class="btn btn-danger btn-sm" @click="deleteAcademic(academic.academicId)"><i class="fa-solid fa-trash-can"></i></button>
                        <button class="btn btn-warning btn-sm ml-2" @click="toEdit(academic.academicId)"><i class="fa-solid fa-pen"></i></button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>
