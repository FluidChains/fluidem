import React, { useEffect } from "react";
import { Route, Switch } from "react-router";
import ExceptionsList from "./ExceptionsList";
import ExceptionDetail from "./ExceptionDetail";
import { useLocation } from "react-router-dom";

export const ErrorLogViewer = (props) => {
  const location = useLocation();
  const apiUrl = props.apiUrl;
  const funException = props.handleException;

  return (
    <div>
      <Switch>
        <Route path="**/exception-detail/:id" component={ExceptionDetail} />
        <Route exact path="*" component={ExceptionsList} />
      </Switch>
    </div>
  );
};
