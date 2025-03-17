<script setup lang="ts">
import { onMounted, onBeforeUnmount, nextTick, ref } from 'vue'
import * as Session from "supertokens-web-js/recipe/session"
import StudentDropdowns from './StudentDropdowns.vue'
import TeacherDropdowns from './TeacherDropdowns.vue'
import AdminDropdowns from './AdminDropdowns.vue'
import { getUserInfo } from '@/authService' 

const userData = ref({})
 
const dropdownItems = document.querySelectorAll('.dropdown__item')
const dropdownContainers = document.querySelectorAll('.dropdown__container')
const handleResize = () => removeStylesOnResize(dropdownContainers, dropdownItems)
window.addEventListener('resize', handleResize)

onBeforeUnmount(() => {
  window.removeEventListener('resize', handleResize)
})
onMounted(() => {
  nextTick(() => {
    getUserInfo().then((data) => {
      userData.value = data.userData
    })
    showMenu('nav-toggle', 'nav-menu')


    dropdownItems.forEach((item) => {
      const dropdownButton = item.querySelector('.dropdown__button')
      if (dropdownButton) {
        dropdownButton.addEventListener('click', () => {
          const activeDropdown = document.querySelector('.show-dropdown')
          if (activeDropdown && activeDropdown !== item) {
            toggleItem(activeDropdown)
          }
          toggleItem(item)
        })
      }
    })


  })
})

const onLogout = async () => {
  await Session.signOut()
  window.location.reload()
}

function showMenu(toggleId: string, navId: string) {
  const toggle = document.getElementById(toggleId)
  const nav = document.getElementById(navId)
  const blogArrow = document.getElementById('blog_arrow')
  const productsArrow = document.getElementById('products_arrow')

  if (toggle && nav) {
    toggle.addEventListener('click', () => {
      nav.classList.toggle('show-menu')
      toggle.classList.toggle('show-icon')
      blogArrow?.classList.remove('hidden')
      productsArrow?.classList.remove('hidden')
    })
  }
}

function toggleItem(item: Element) {
  const dropdownContainer = item.querySelector('.dropdown__container') as HTMLElement
  if (!dropdownContainer) return

  if (item.classList.contains('show-dropdown')) {
    dropdownContainer.removeAttribute('style')
    item.classList.remove('show-dropdown')
  } else {
    dropdownContainer.style.height = dropdownContainer.scrollHeight + 'px'
    item.classList.add('show-dropdown')
  }
}

function removeStylesOnResize(dropdownContainers: NodeListOf<Element>, dropdownItems: NodeListOf<Element>) {
  const mediaQuery = matchMedia('(min-width: 1118px)')
  if (mediaQuery.matches) {
    dropdownContainers.forEach((dc) => {
      (dc as HTMLElement).removeAttribute('style')
    })
    dropdownItems.forEach((di) => {
      di.classList.remove('show-dropdown')
    })
    const blogArrow = document.getElementById('blog_arrow')
    const productsArrow = document.getElementById('products_arrow')
    blogArrow?.classList.add('hidden')
    productsArrow?.classList.add('hidden')
  }
}
</script>

<template>
  <header class="header">
    <nav class="nav container">
      <!-- Логотип -->
      <div class="d-flex justify-content-between align-items-center h-100">
        <a href="/" class="nav__logo">
          <img src="../../assets/logo-bg.svg" alt="Logo" />
        </a>
      </div>

      <div class="nav__toggle ms-auto my-auto" id="nav-toggle">
        <i class="ri-menu-line nav__toggle-menu"></i>
        <i class="ri-close-line nav__toggle-close"></i>
      </div>

      <div class="nav__menu" id="nav-menu">
        <ul class="nav__list">
          <StudentDropdowns v-if="userData.userRole === 'student'" />
          <TeacherDropdowns v-if="userData.userRole === 'teacher'" />
          <AdminDropdowns v-if="userData.userRole === 'admin'" />

          <li>
            <a href="/profile/edit" class="nav__link fw-bold">Profile</a>
          </li>
          <li>
            <a href="/logout" @click.prevent="onLogout" class="nav__link fw-bold">Logout</a>
          </li>
        </ul>

        <ul class="nav-footer">
          <li>
            <a href="#" class="nav__link">Imressum</a>
          </li>
          <li>
            <a href="#" class="nav__link">Datenschutz</a>
          </li>
          <li>
            <a href="#" class="nav__link">&copy; 2023 Loremipsum GmbH</a>
          </li>
        </ul>
      </div>
    </nav>
  </header>
</template>

