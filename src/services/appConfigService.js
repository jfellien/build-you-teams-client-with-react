const appConfig = {
    auth: {
        clientId: '',
        tenatId: '',
        authority: ''
    },
    graph : {
        uri: '',
        userScopes: '',
        acsScopes: ''
    }
}

const getAppConfig = async () => {
    const response = await fetch(`/api/app-config`);
    const appConfig = await (response).json();

    return appConfig;
}

export default getAppConfig;