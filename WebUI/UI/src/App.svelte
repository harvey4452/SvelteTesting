<script>
	import ImportedHeader from './header.svelte';
	import Login from './Login.svelte';

	let API_URL = "http://localhost:5035/";
	let isValid;

	function GetIsValid(){
		let newUrl = API_URL+"api/GymApp/GetCredentials?pEmail="+email+"&pPassword="+password;
		fetch(newUrl)
		.then((response)=>response.json())
		.then((data)=>{
			alert(data);
			isValid=data;
		});
	}

</script>

<div>
	<ImportedHeader />
	<Login />
</div>

<style>

</style>
