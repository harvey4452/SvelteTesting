<script>
    let displayMessage = 'false';
    let Email;
    let Password;
    let Fname;
    let Lname;
    let AccessLevel;

    let API_URL = "http://localhost:5035/";

    function CreateAccount(){
        let newUrl = API_URL+"api/GymApp/CreateAccount?pEmail="+Email+"&pPassword="+Password+"&pFirstName="+Fname+"&pLastName="+Lname+"&pAccessLevel="+AccessLevel;
        fetch(newUrl,{method:"POST"})
		.then((response)=>response.json())
		.then((data)=>{
			if (data == "Account created!"){
                displayMessage = 'success';
            }
            else if(data == "Account with that email address already exists!"){
                displayMessage = 'error';
            }
		});
    }


</script>

<div class="CreateAccountContainer">
    <div class="row">
        <div class="col align-self-center">
            <input class="inputBox" bind:value={Email} placeholder="Email" />
        </div>
        <div class="col align-self-center">
            <input class="inputBox" bind:value={Password} placeholder="Password" />
        </div>
        <div class="col align-self-center">
            <input class="inputBox" bind:value={Fname} placeholder="Forename" />
        </div>
        <div class="col align-self-center">
            <input class="inputBox" bind:value={Lname} placeholder="Surname" />
        </div>
        <div class="col align-self-center">
            <input class="inputBox" bind:value={AccessLevel} placeholder="Access level" />
        </div>
        {#if displayMessage == 'error'}
            <p class="ErrorMessage">Account with email already exists in our database!</p>
        {:else if displayMessage == 'success'}
            <p class="SuccessMessage">Account created!</p>
        {/if}
        <div class="col align-self-right">
            <button class="buttonStyle" on:click={CreateAccount}>Create new account</button>
        </div>
    </div>
</div>
<p>Admin homepage</p>

<style>
	.ErrorMessage{
        color:red;
    }
    .SuccessMessage{
        color:green;
    }
    .CreateAccountContainer{
        text-align: center;
    }
    	.inputBox{
		width: 35em;
		height: 3em;
		border-radius: 0.5em;
	}
</style>