import React, {useState, useEffect} from 'react';

let getAppConfig = async () => {
  const response = await fetch(`/api/app-config`);
  const config = await (response).json();

  return config;
}

function App() {
  const [appConfig, setAppConfig] = useState();

  useEffect(() => {
    (async () => {
      const appConfig = await getAppConfig();
      setAppConfig(appConfig);
    })();
  });

  return <div>Hello {appConfig}</div>;
}

export default App;
