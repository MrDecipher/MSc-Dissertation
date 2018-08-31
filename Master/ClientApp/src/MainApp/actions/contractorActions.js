import { contractorActionTypes } from "../actionTypes";
import { authenticationService } from "../services";
import { alertActions } from "./alertActions";

export const contractorActions = {
    login,
    logout
};

function login(email, password) {
    return dispatch => {
        dispatch(request());
        return authenticationService.login(email, password)
                .then((res) => {
                    if(res.status == 202) {
                        dispatch(success(res.body.user));
                    } else {
                        throw "Invalid login credentials";
                    }
                })
                .catch((error) => {
                    dispatch(failure(error));
                    dispatch(alertActions.error(error));
                });
    };

    function request() {
        return { type: contractorActionTypes.LOGIN_REQUEST };
    }

    function success(user) {
        return { type: contractorActionTypes.LOGIN_SUCCESS, user };
    }

    function failure(error) {
        return { type: contractorActionTypes.LOGIN_ERROR, error };
    }
};

function logout() {
    authenticationService.logout();
    return { type: contractorActionTypes.LOGOUT };
}