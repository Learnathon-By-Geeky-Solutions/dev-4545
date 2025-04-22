import _ from 'lodash';
import { QueryParams } from '@models/utils-model';

export const isProd = process.env.MODE === 'production';

export const formatQueryParams = (params: QueryParams) => {
  const queryParams: string[] = [];
  
  Object.entries(params).forEach(([key, value]) => {
    if (value === undefined || value === null || value === '') return;
    
    if (Array.isArray(value)) {
      queryParams.push(`${key}=${value.join(',')}`);
    } else {
      queryParams.push(`${key}=${value.toString()}`);
    }
  });
  
  return queryParams.join('&');
};

export const toRemoveEmptyValue = (obj: QueryParams) => {
  return _.omitBy(obj, value =>
    _.isUndefined(value) || ((_.isString(value) && _.isEmpty(value)) || (_.isArray(value) && _.isEmpty(value)))
  );
};