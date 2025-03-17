<script>
import { defineComponent } from 'vue';
import Header from '@/components/navbar/Header.vue';
import { getUserInfo } from "@/authService";
import * as Session from "supertokens-web-js/recipe/session";

export default defineComponent({
    components:{
        Header,
    },
    data() {
        return {
            universityName: "",
        };
    },
  methods: {
    async submitUniversity() {
        if (!this.universityName.trim()) {
            alert("Please enter a university name.");
            return;
        }
        try {
            const response = await fetch("http://localhost/api/universities", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ universityName: this.universityName }),
            });
            if (!response.ok) {
                throw new Error("Failed to create university");
            }
            const result = await response.json();
            alert("University created successfully!");
        } catch (error) {
                console.error("Error creating university:", error);
                alert("Error creating university");
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
                                <strong>Create University</strong>
                            </h2>
                            <p>With this form u can create new Uni</p>
                            <div class="card-block">
                                <form @submit.prevent="submitUniversity">
                                    <div class="md-form">
                                        <input type="text" v-model="universityName" name="universityName" id="form-uniname" class="form-control" />
                                        <label for="form-uniname">Name of University</label>
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