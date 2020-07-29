import React from "react";
import { Route, Switch } from "react-router";
import PropTypes from "prop-types";
import ExceptionsList from "./ExceptionsList";
import ExceptionDetail from "./ExceptionDetail";

const ErrorLogViewer = props => {
  const { apiUrl, errorHandler } = props;
  return (
    <div>
      <Switch>
        <Route
          path="**/detail/:id"
          render={cprops => (
            <ExceptionDetail
              {...cprops}
              apiUrl={apiUrl}
              errorHandler={errorHandler}
            />
          )}
        />
        <Route
          exact
          path="*"
          render={cprops => (
            <ExceptionsList
              {...cprops}
              apiUrl={apiUrl}
              errorHandler={errorHandler}
            />
          )}
        />
      </Switch>
    </div>
  );
};

ErrorLogViewer.propTypes = {
  apiUrl: PropTypes.string,
  errorHandler: PropTypes.func,
};

ErrorLogViewer.defaultProps = {
  apiUrl: "api/error-log",
  errorHandler: e => {
    throw e;
  },
};

export default ErrorLogViewer;
