<template>

 <div style="display: flex; flex-direction: column; min-height: 100%; min-width: 100%">

  <Toast />

  <div style="cursor: pointer" @click="test">login</div>

  <router-view > </router-view>


 </div>

</template>

<script lang="ts">
import {Router} from "vue-router";
import {singleton} from "../singleton";

export default {

 mounted() {

  singleton.Instance.mitter.off('err-toast')


  singleton.Instance.mitter.on('err-toast',(msg)=>{

  this.$toast.add({severity:'error', summary: 'Error', detail:msg, life: 3000});

  }
  )

  },




  methods:{


  test(){



  this.$router.push('/auth/login')
  }

  },


}



</script>

<style scoped>

</style>