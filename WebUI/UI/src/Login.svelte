<script>
    import CustomerHomepage from './CustomerHomepage.svelte';
    import AdminHomepage from './AdminHomepage.svelte';

    let email;
    let password;

    let API_URL = "http://localhost:5035/";

    let loggedIn = 'false';
    let displayErrorMessage = 'false';

	let isCustomer = 'false';
	let isAdmin = 'false';

	function VerifyCredentials(){
		let newUrl = API_URL+"api/GymApp/GetCredentials?pEmail="+email+"&pPassword="+password;
		fetch(newUrl)
		.then((response)=>response.json())
		.then((data)=>{
			if (data == "true"){
				loggedIn = "true";
				GetAccessLevel();
			}
			else{
				displayErrorMessage = "true";
			}
		});
	}
	function GetAccessLevel(){
		let newUrl = API_URL+"api/GymApp/GetAccessLevel?pEmail="+email;
		fetch(newUrl)
		.then((response)=>response.json())
		.then((data)=>{
			if (data == "Customer"){
				isCustomer = 'true';
			}
			else if (data == "Admin"){
				isAdmin = 'true';
			}
		});
	}

</script>

<div>
    {#if loggedIn == 'true'}
		{#if isCustomer == 'true'}
            <CustomerHomepage CustomerEmail={email}/>
        {:else if isAdmin == 'true'}
            <AdminHomepage/>
        {/if}
	{:else}
		<div class="loginContainer">
			<div class="row">
				<div class="col align-self-center">
					<input class="inputBox" bind:value={email} placeholder="Email" />
				</div>
				<div class="col align-self-center">
	    			<input class="inputBox" bind:value={password} placeholder="Password" />
				</div>
				{#if displayErrorMessage == 'true'}
            		<p class="ErrorMessage">Incorrect email or password!</p>
        		{/if}
				<div class="col align-self-right">
        			<button class="buttonStyle" on:click={VerifyCredentials}>Login</button>
				</div>
			</div>
		</div>
	{/if}
</div>

<style>
	.ErrorMessage{
        color:red;
    }
	.loginContainer{
		text-align: center;
	}
	.inputBox{
		width: 35em;
		height: 3em;
		border-radius: 0.5em;
	}
	.buttonStyle{
		border-radius: 0.5em;
		background-color: #ADD8E6;
		color:white;
		font-size:1.5em;
		font-weight:600;
		height: 2.25em;
		width: 4.5em;
	}
</style>

