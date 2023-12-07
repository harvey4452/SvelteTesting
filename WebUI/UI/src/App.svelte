<script>
	import ImportedHeader from './header.svelte';
<<<<<<< HEAD
	import Login from './Login.svelte';
=======

	let API_URL = "http://localhost:5035/";
	let isValid;

	function GetIsValid(){
		let newUrl = API_URL+"api/GymApp/GetCredentials?pEmail="+email+"&pPassword="+password;
		alert(newUrl);
		fetch(newUrl)
		.then((response)=>response.json())
		.then((data)=>{
			alert(data);
			isValid=data;
		});
	}
>>>>>>> c78e83ca2aa9d7b25ef8f873614854d46115ef31

</script>

<div>
	<ImportedHeader />
	<Login />
</div>

<style>

</style>
