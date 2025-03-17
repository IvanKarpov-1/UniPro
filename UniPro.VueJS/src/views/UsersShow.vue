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
            users: [],
        };
    },
    mounted() {
        this.fetchUsers();
    },
    methods: {
        async fetchUsers() {
            try {
                const response = await fetch("http://localhost/api/users");
                if (!response.ok) {
                    throw new Error("Failed to fetch users");
                }
                this.users = await response.json();
                console.log(this.users);
            } catch (error) {
                console.error("Error fetching users:", error);
            }
        },
        async deleteUser(id) {
            if (!confirm("Are you sure you want to delete this user?")) {
                return;
            }
            try {
                const response = await fetch(`http://localhost/api/users/${id}`, {
                    method: "DELETE",
                });
                if (!response.ok) {
                    throw new Error("Failed to delete user");
                }
                this.users = this.users.filter(user => user.userId !== id);
            } catch (error) {
                console.error("Error deleting user:", error);
            }
        },
        toEdit(id){
            this.$router.push({ path: `/user/${id}` });
        }
    },
});
</script>

<template>
    <Header />
    <div class="container mt-4">
        <h2>Users List</h2>
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Full Name</th>
                    <th>Phone Number</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="user in users" :key="user.userId">
                    <td>{{ user.userId }}</td>
                    <td>{{ user.firstName }} {{ user.patronymic }} {{ user.lastName }}</td>
                    <td>{{ user.phoneNumber }}</td>
                    <td>
                        <button class="btn btn-danger btn-sm" @click="deleteUser(user.userId)"><i class="fa-solid fa-trash-can"></i></button>
                        <button class="btn btn-warning btn-sm ml-2" @click="toEdit(user.userId)"><i class="fa-solid fa-pen"></i></button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>
