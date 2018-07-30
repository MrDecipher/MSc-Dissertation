import React from "react";
import ReactDOM from "react-dom";

import { AppContainer } from "react-hot-loader";
import App from "./MainApp/App";

const render = Component => {
    ReactDOM.render(
        <AppContainer>
            <Component />
        </AppContainer>,
        document.getElementById('app-container')
    );
}

render(App);

if (module.hot) {
    module.hot.accept('./MainApp/App', () => {
      const NextApp = require('./MainApp/App').default;
      render(NextApp);
    });
  }