
<script lang="ts">
import { defineComponent, onMounted, onUnmounted } from 'vue';
export default defineComponent({
    setup() {
        const loadScript = (src: string) => {
            const script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = src;
            script.id = 'supertokens-script';
            script.onload = () => {
                (window as any).supertokensUIInit("supertokensui", {
                    appInfo: {
                        appName: "UniPro",
                        apiDomain: "http://localhost",
                        websiteDomain: "http://localhost:5173", // container port 8080 
                        apiBasePath: "/api/auth",
                        websiteBasePath: "/auth"
                    },
                    recipeList: [ 
                        (window as any).supertokensUIEmailPassword.init(
                        // ===== TO CHANGE REDIRECT =====
                        //     {
                        //     onHandleEvent: (event: any) =>{
                        //         if (event.action === "SUCCESS") {
                        //             window.location.href = '/profile';
                        //         }
                        //     }
                        // }
                    ),
                        (window as any).supertokensUISession.init(),
                    ],
                });
            };
            document.body.appendChild(script);
        };

        onMounted(() => {
            loadScript('https://cdn.jsdelivr.net/gh/supertokens/prebuiltui@v0.47.0/build/static/js/main.00ec3e91.js');
        });

        onUnmounted(() => {
            const script = document.getElementById('supertokens-script');
            if (script) {
                script.remove();
            }
        });
    },
});
</script>

<template>
    <div id="supertokensui" />
</template>
