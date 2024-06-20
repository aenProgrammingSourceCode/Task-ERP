
async function getToken() {
    try {
        const tokenResponse = await fetch('/api/Authen/GetToken');
        if (!tokenResponse.ok) {
            throw new Error('Failed to fetch token');
        }
        const token = await tokenResponse.text();
        return token; // Return the token value
    } catch (error) {
        console.error('Error fetching token:', error.message);
        throw error;
    }
}
async function getAllDataWithParameter(url, jsonData) {
    try {
        const token = await getToken();
        const response = await fetch('/api/'+url, {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(jsonData)
        });
        
        if (!response.ok) {
            throw new Error('Network response was not ok.');
        }
        return await response.json(); 
    } catch (error) {
        console.error("Error fetching data from the server: " + error.message);
        throw error; // Rethrow the error for handling in the calling function
    }
}

async function getByData(url, successCallback) {
        const token = await getToken();
        await $.ajax({
            url: '/api/' + url,
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            },
            dataType: 'json',
            contentType: 'application/json;charset-utf-8',
            success: successCallback,
            error: function (er) {
                var errorSycronim = JSON.stringify(er);
                alert("error: " + errorSycronim);
            }
        });
}

async function deleteDataById(url, successCallback) {
    const token = await getToken();
    await $.ajax({
        url: '/api/' + url,
        type: 'POST',
        headers: {
            'Authorization': 'Bearer ' + token
        },
        contentType: 'application/json;charset=utf-8',  // Corrected typo here
        success: successCallback,
        error: function (er) {
            var errorSycronim = JSON.stringify(er);
            console.log("error: " + errorSycronim);
        }
    });
}


async function postDataWithParameterAndReturn(url, jsonData, successCallback) {
        const token = await getToken();
        await $.ajax({
            url: '/api/' + url,
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + token // Set the Authorization header with the token
            },
            contentType: 'application/json',
            data: JSON.stringify(jsonData),
            success: successCallback,
            error: function (er) {
                var errorSycronim = JSON.stringify(er);
                alert("error: " + errorSycronim);
            }
        });
    }
async function postDataWithoutParameterAndReturn(url) {
        const token = await getToken();
        await $.ajax({
            url: '/api/' + url,
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + token // Set the Authorization header with the token
            },
            contentType: 'application/json',
            error: function (er) {
                var errorSycronim = JSON.stringify(er);
                alert("error: " + errorSycronim);
            }
        });
    }
async function postArrayDataWithParameter(url, postArr, successCallback) {
        try {
            const token = await getToken();
            await $.ajax({
                url: '/api/' + url,
                type: 'POST',
                headers: {
                    'Authorization': 'Bearer ' + token // Set the Authorization header with the token
                },
                dataType: 'json',
                data: JSON.stringify(postArr),
                contentType: 'application/json; charset=utf-8',
                success: successCallback,
                error: function (er) {
                    var errorSycronim = JSON.stringify(er);
                    alert("error: " + errorSycronim);
                }
            });
        } catch (error) {
            console.error("Error updating approval status: " + error.message);
        }
    }

 